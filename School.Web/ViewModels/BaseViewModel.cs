using School.Models;
namespace School.Web.ViewModels
{
    public class BaseViewModel
    {

        public string Guid { get; set; }

        public string DateCreated { get; set; }

        public string DateModified { get; set; }

        public ObjectState ObjectState { get; set; }

        public string MessageToClient { get; set; }

        public byte[] RowVersion { get; set; }
    }
}