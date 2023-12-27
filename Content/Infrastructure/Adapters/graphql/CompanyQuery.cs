using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Adapters.graphql;

public class CompanyQuery
{
    
    [UsePaging]
    [UseFiltering]
    public IQueryable<Company> GetCompaniesPaginated([Service] PersistenceContext dbContext) =>
        dbContext.Companies.Include(company => company.CommercialSegment);
    
    [UseFiltering]
    public IQueryable<Company> GetCompanies([Service] PersistenceContext dbContext) =>
        dbContext.Companies.Include(company => company.CommercialSegment);
}