//namespace TravelMateAPI.Services.Contract
//{
using BusinessObjects.Entities;
using TravelMateAPI.Services.Contract;

public interface IContractService
{
        Task<ContractDTO> CreateContract(int travelerId, int localId, string tourId,string  Location, string details, string status, string travelerSignature, string localSignature);
        Task<ContractDTO> CreateContractPassLocal(int travelerId, int localId, string tourId,string scheduleId, string Location, string details, string status, string travelerSignature);
        ContractDTO FindContractInMemory(int travelerId, int localId, string tourId, string scheduleId);
        Task UpdateStatusToCompleted(int travelerId, int localId, string tourId, string scheduleId);
        Task UpdateStatusToCancelled(int travelerId, int localId, string tourId, string scheduleId);
        Task SaveContractToDatabase(int travelerId, int localId, string tourId, string scheduleId);
        Task<bool> VerifyContractIntegrityAsync(int travelerId, int localId, string tourId, string scheduleId);
        Task<int> GetContractCountAsLocalAsync(int userId);
        Task<int> GetContractLocationCountAsync(string location);
        Task<int> GetTotalContractCountAsync();
        Task<List<TravelerContractDTO>> GetContractsByTravelerAsync(int travelerId);
        Task<List<LocalContractDTO>> GetContractsByLocalAsync(int travelerId);
        Task<string> CheckContractStatusAsync(int travelerId, string tourId, string scheduleId);
        Task<List<string>> GetLocationsByTravelerIdAsync(int travelerId);
        Task<List<Location>> GetTopLocationsDetailsAsync(int top);
}
//}
