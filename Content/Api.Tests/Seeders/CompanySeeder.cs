using Domain.Entities;
using Domain.Ports;
using Domain.Tests.BuilderEntities;

namespace Api.Tests.Seeders
{
    internal class CompanySeeder : IDataSeeder
    {
        private readonly IGenericRepository<Company> _companyRepository;
        public CompanySeeder(IGenericRepository<Company> companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public void Seed()
        {
            var identity = new IdentityBuilder().WithDocumentType("CC").WithLegalIdentifier("32841277").Build();
            var authorizedA = new AuthorizedAgentBuilder().WithIdentity(identity).WithEmail("jedev@gmail.com").Build();
            var company = new CompanyBuilder()
                .WithAuthorizedAgent(authorizedA)
                .WithName("uniqueName")
                .WithLegalIdentifier("328412777")
                .WithHostname("unique.com")
                .Build();
            _companyRepository.AddAsync(company).Wait();
            var identity2 = new IdentityBuilder().WithDocumentType("CC").WithLegalIdentifier("1066865371").Build();
            var authorizedA2 = new AuthorizedAgentBuilder().WithIdentity(identity2).WithEmail("jedev2@gmail.com").Build();
            var company2 = new CompanyBuilder()
                .WithAuthorizedAgent(authorizedA2)
                .WithName("uniqueName2")
                .WithLegalIdentifier("1066865371")
                .WithHostname("unique2.com")
                .Build();
            _companyRepository.AddAsync(company2).Wait();
        }
    }
}
