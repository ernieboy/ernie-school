var ObjectState = {
    Unchanged: 0,
    Added: 1,
    Modified: 2,
    Deleted: 3
};

var examItemMapping = {
    'ExamsTaken': {
        key: function (examination) {
            return ko.utils.unwrapObservable(examination.ExaminationId);
        },
        create: function (options) {
            return new ExaminationViewModel(options.data);
        }
    }
};

var dataConverter = function (key, value) {
    if (key === 'RowVersion' && Array.isArray(value)) {
        var str = String.fromCharCode.apply(null, value);
        return btoa(str);
    }
    return value;
};

ko.bindingHandlers.datepicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        //initialize datepicker with some optional options
        var options = allBindingsAccessor().datepickerOptions || {};
        $(element).datepicker(options);

        //handle the field changing
        ko.utils.registerEventHandler(element, "change", function () {
            var observable = valueAccessor();
            observable($(element).datepicker("getDate"));
        });

        //handle disposal (if KO removes by the template binding)
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).datepicker("destroy");
        });

    },
    //update the control when the view model changes
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor()),
            current = $(element).datepicker("getDate");

        if (value - current !== 0) {
            $(element).datepicker("setDate", value);
        }
    }
};

ExaminationViewModel = function (data) {
    var self = this;
    ko.mapping.fromJS(data, examItemMapping, self);

    self.flagExamAsEdited = function () {
        if (self.ObjectState() != ObjectState.Added) {
            self.ObjectState(ObjectState.Modified);
        }
        return true;
    },
    self.marksObtainedAsPercentage = ko.computed(function () {
        return ((self.MarksObtained() / self.MaximumMarks()) * 100).toFixed(1);
    }),
    self.examGrade = ko.computed(function () {
        var grade = "U";
        if (self.marksObtainedAsPercentage() >= 95) {
            grade = "A*";
        } else if (self.marksObtainedAsPercentage() >= 80 && self.marksObtainedAsPercentage() <= 95) {
            grade = "A";
        } else if (self.marksObtainedAsPercentage() >= 70 && self.marksObtainedAsPercentage() <= 80) {
            grade = "B";
        } else if (self.marksObtainedAsPercentage() >= 60 && self.marksObtainedAsPercentage() <= 70) {
            grade = "C";
        } else if (self.marksObtainedAsPercentage() >= 50 && self.marksObtainedAsPercentage() <= 60) {
            grade = "D";
        } else if (self.marksObtainedAsPercentage() >= 40 && self.marksObtainedAsPercentage() <= 50) {
            grade = "E";
        } else if (self.marksObtainedAsPercentage() >= 30 && self.marksObtainedAsPercentage() <= 40) {
            grade = "F";
        } else if (self.marksObtainedAsPercentage() >= 20 && self.marksObtainedAsPercentage() <= 30) {
            grade = "F";
        }
        return grade;

    });
};

StudentViewModel = function (data) {
    var self = this;
    ko.mapping.fromJS(data, examItemMapping, self); // self is the view model that we want to populate with the properties the ko mapping plugin creates from the properties in the serverside view model

    self.save = function () {
        $.ajax({
            url: "/Students/Save",
            type: "POST",
            data: ko.toJSON(self, dataConverter),
            contentType: "application/json",
            success: function (data) {
                if (data.studentViewModel != null) {
                    ko.mapping.fromJS(data.studentViewModel, {}, self);
                    //$('#MessageToClient').text('Student saved successfully!');
                }
                if (data.newLocation != null) {
                    window.location = data.newLocation;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (XMLHttpRequest.status == 400) {                    
                    self.handleServerError400(XMLHttpRequest.responseText);
                } else if (XMLHttpRequest.status == 500) {
                    self.handleServerError500(XMLHttpRequest.responseText);
                } else {
                    $('#MessageToClient').text('An error has occurred while processing your request. Please ensure that you have entered correct data in all fields, especially date fields.');
                }
            }
        });
    }
    self.handleServerError400 = function (responseText) {
        if (responseText.indexOf('Entities may have been modified or deleted since entities were loaded') > 0) {
            $('#MessageToClient').text('Someone else has modified this record in the database since you retrieved it. Your changes have been discarded. What you see now are the current values in the database. ');
            return;
        }        
        $('#MessageToClient').text(responseText);
    }
    self.handleServerError500 = function (responseText) {
        if (responseText == null) return;
        if (responseText.indexOf('Cannot insert duplicate key row in object') > 0) {
            if (responseText.indexOf('AK_ExaminationItem') > 0) {
                $('#MessageToClient').text('Please ensure that all exams are different, for example, you cannot have an exam with the same name twice for the same student.');
                return;
            }
        } else if (responseText.indexOf('not recognized as a valid DateTime') > 0) {
            $('#MessageToClient').text('One of your dates is invalid. Please check all dates and make sure that they are correct. ');
            return;
        }
        $('#MessageToClient').text('An error has occurred while processing your request. Please ensure that you have entered correct data in all fields, especially date fields.');
    }

    self.flagStudentAsEdited = function () {
        if (self.ObjectState() !== ObjectState.Added) {
            self.ObjectState(ObjectState.Modified);
        }
        return true;
    }
    self.addStudentExam = function () {
        var exam = new ExaminationViewModel({ Subject: "", MaximumMarks: 0, MarksObtained: 0, ExaminationId: 0, DateTaken: "", ObjectState: ObjectState.Added });
        self.ExamsTaken.push(exam);
    },
    self.overallMarks = ko.computed(function () {
        var totalMaximumMarks = 0;
        var totalMarksObtained = 0;
        var percent = 0;
        var result = "";
        ko.utils.arrayForEach(self.ExamsTaken(), function (examination) {
            totalMaximumMarks += parseInt(examination.MaximumMarks());
            totalMarksObtained += parseFloat(examination.MarksObtained());
        });
        percent = (totalMarksObtained / totalMaximumMarks) * 100;
        result = "Total for all exams:  " + totalMarksObtained + "/" + totalMaximumMarks + "(" + percent.toFixed(1) + "%)."
        return result;
    }),
    self.deleteExamItem = function (examination) {
        self.ExamsTaken.remove(this);
        if (examination.ExaminationId() > 0 && self.ExamItemsToDelete.indexOf(examination.ExaminationId()) == -1) {
            self.ExamItemsToDelete.push(examination.ExaminationId);
        }
    };
};




$("#studentEditForm").validate({
    submitHandler: function () {
        studentViewModel.save();
    },
    rules: {
        firstName: {
            required: true,
            maxlength: 30,
            alphaonly: true
        },
        Subject: {
            required: true,
            maxlength: 50
        },
        MaximumMarks: {
            required: true,
            number: true,
            range: [20, 180]
        }
    },

    messages: {
        firstName: {
            required: "You must enter a first name",
            maxlength: "First names must be 30 characters or shorter",
            alphaonly: "First name must consist of letters only"
        },
        MaximumMarks: {
            required: "Please specify maximum number of marks for this exam",
            number: "Maximum number of marks must be a number",
            range: "Maximum number of marks must be between 20 and 180"
        }
    },
    tooltip_options: {
        firstName: {
            placement: 'right'
        },
        MaximumMarks: {
            placement: 'right'
        }
    }
});

$.validator.addMethod("alphaonly",
    function (value) {
        return /^[A-Za-z]+$/.test(value);
    });