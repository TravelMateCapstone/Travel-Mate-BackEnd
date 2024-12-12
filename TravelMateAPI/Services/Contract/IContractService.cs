//namespace TravelMateAPI.Services.Contract
//{
using TravelMateAPI.Services.Contract;

public interface IContractService
{
        Task<ContractDTO> CreateContract(int travelerId, int localId, string tourId,string  Location, string details, string status, string travelerSignature, string localSignature);
        Task<ContractDTO> CreateContractPassLocal(int travelerId, int localId, string tourId, string Location, string details, string status, string travelerSignature);
        ContractDTO FindContractInMemory(int travelerId, int localId, string tourId);
        Task UpdateStatusToCompleted(int travelerId, int localId, string tourId);
        Task UpdateStatusToCancelled(int travelerId, int localId, string tourId);
        Task SaveContractToDatabase(int travelerId, int localId, string tourId);
        Task<bool> VerifyContractIntegrityAsync(int travelerId, int localId, string tourId);
        Task<int> GetContractCountAsLocalAsync(int userId);
        Task<int> GetContractLocationCountAsync(string location);
        Task<List<TravelerContractDTO>> GetContractsByTravelerAsync(int travelerId);
        Task<List<LocalContractDTO>> GetContractsByLocalAsync(int travelerId);
        Task<string> CheckContractStatusAsync(int travelerId, string tourId);
}
//}
