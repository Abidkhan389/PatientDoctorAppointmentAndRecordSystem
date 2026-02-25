
namespace PatientDoctor.Application.Contracts.Persistance.IIdentityRepository
{
    public interface IIdentityRepository
    {
        // Task<IResponse> GetAllProducts(GetProductList model);
        Task<IResponse> GetUserById(GetUserById Id);
        Task<IResponse> GetDoctorFee(GetDoctorFee Id);
        Task<IResponse> LoginUserAsync(LoginUserCommand model);
        Task<IResponse> ActiveInActiveUser(ActiveInActiveIdentity model);
        Task<IResponse> GetAllByProc(GetUserList model);
        Task<IResponse> GetAllRoles();
        Task<IResponse> GetAllDoctors();
        Task<IResponse> AddEditUser(AddEditUserWithCreatedOrUpdatedById model);

        Task<UserRefreshTokenDto> GetRefreshTokenAsync(string refreshToken);
        Task RevokeAllTokensAsync(string userId);


        Task<AuthTokenResultDto> GenerateTokensAsync(string userId);
        Task MarkTokenAsUsedAsync(string refreshToken);
        Task<string?> GetUserIdByRefreshTokenAsync(string refreshToken);
        Task<IResponse> GoogleLoginAsync(string email, string name, string providerKey, CancellationToken cancellationToken);

    }
}
