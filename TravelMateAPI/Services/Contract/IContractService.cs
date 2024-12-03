//namespace TravelMateAPI.Services.Contract
//{
    public interface IContractService
    {
        Task<ContractDTO> CreateContract(int travelerId, int localId, int tourId, string details, string status, string travelerSignature, string localSignature);
        ContractDTO FindContractInMemory(int travelerId, int localId, int tourId);
        void UpdateStatusToCompleted(int travelerId, int localId, int tourId);
        void UpdateStatusToCancelled(int travelerId, int localId, int tourId);
        Task SaveContractToDatabase(int travelerId, int localId, int tourId);
        Task<bool> VerifyContractIntegrityAsync(int travelerId, int localId, int tourId);
        Task<int> GetContractCountAsLocalAsync(int userId);
    }
//}
