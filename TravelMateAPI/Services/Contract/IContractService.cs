//namespace TravelMateAPI.Services.Contract
//{
    public interface IContractService
    {
        Task<ContractDTO> CreateContract(int travelerId, int localId, string tourId,string  Location, string details, string status, string travelerSignature, string localSignature);
        ContractDTO FindContractInMemory(int travelerId, int localId, string tourId);
        Task UpdateStatusToCompleted(int travelerId, int localId, string tourId);
        Task UpdateStatusToCancelled(int travelerId, int localId, string tourId);
        Task SaveContractToDatabase(int travelerId, int localId, string tourId);
        Task<bool> VerifyContractIntegrityAsync(int travelerId, int localId, string tourId);
        Task<int> GetContractCountAsLocalAsync(int userId);
        Task<int> GetContractLocationCountAsync(string location);

    }
//}
