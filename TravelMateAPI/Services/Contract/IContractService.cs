//namespace TravelMateAPI.Services.Contract
//{
    public interface IContractService
    {
        Task<ContractDTO> CreateContract(int travelerId, int localId, string tourId, string details, string status, string travelerSignature, string localSignature);
        ContractDTO FindContractInMemory(int travelerId, int localId, string tourId);
        void UpdateStatusToCompleted(int travelerId, int localId, string tourId);
        void UpdateStatusToCancelled(int travelerId, int localId, string tourId);
        Task SaveContractToDatabase(int travelerId, int localId, string tourId);
        Task<bool> VerifyContractIntegrityAsync(int travelerId, int localId, string tourId);
        Task<int> GetContractCountAsLocalAsync(int userId);
    }
//}
