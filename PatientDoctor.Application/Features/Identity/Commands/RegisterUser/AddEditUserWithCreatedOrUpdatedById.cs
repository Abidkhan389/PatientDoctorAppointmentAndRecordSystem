
namespace PatientDoctor.Application.Features.Identity.Commands.RegisterUser
{
    public  class AddEditUserWithCreatedOrUpdatedById : TableParam, IRequest<IResponse>
    {
        public AddEditUserCommands addEditUsermodel { get; }
        public Guid UserId { get; }

        public AddEditUserWithCreatedOrUpdatedById(AddEditUserCommands model, Guid UserId)
        {
            this.addEditUsermodel = model;
            this.UserId = UserId;
        }
    }
}
