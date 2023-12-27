using Domain.Entities;
using Domain.Exceptions.Common;
using Domain.Ports;

namespace Domain.Services;

[DomainService]
public class CommercialSegmentService
{
    private readonly IGenericRepository<CommercialSegment> _commercialSegmentRepository;

    public CommercialSegmentService(IGenericRepository<CommercialSegment> commercialSegmentRepository) =>
        _commercialSegmentRepository = commercialSegmentRepository;

    public async Task CreateAsync(CommercialSegment commercialSegment)
    {
        await ValidateExistingName(commercialSegment.Name);
        await _commercialSegmentRepository.AddAsync(commercialSegment);
    }

    public async Task UpdateAsync(Guid id, string name, string description )
    {
        var segmentToUpdate = await GetCommercialSegmentById(id);
        await ValidateExistingName(name);
        segmentToUpdate.Update(name, description);
        await _commercialSegmentRepository.UpdateAsync(segmentToUpdate);
    }

    public async Task DeleteAsync(Guid id)
    {
        var segmentToDelete = await GetCommercialSegmentById(id);
        await _commercialSegmentRepository.DeleteAsync(segmentToDelete);
    }

    private async Task<CommercialSegment> GetCommercialSegmentById(Guid id)
    {
        var segmentToUpdate = await _commercialSegmentRepository.GetByIdAsync(id);
        _ = segmentToUpdate ??
            throw new ResourceNotFoundException(string.Format(Messages.ResourceNotFoundException, nameof(id), id));
        return segmentToUpdate;
    }

    private async Task ValidateExistingName(string name)
    {
        bool alredyExistName = await _commercialSegmentRepository.Exist(
            commercialSegment => commercialSegment.Name == name
        );
        if (alredyExistName)
        {
            string exceptionMessage = string.Format(Messages.AlredyExistException, nameof(name), name);
            throw new ResourceAlreadyExistException(exceptionMessage);
        }
    }
}