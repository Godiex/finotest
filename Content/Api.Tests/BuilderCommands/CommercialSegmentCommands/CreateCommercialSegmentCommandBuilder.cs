using Application.UseCases.ComercialSegments.Commands.CreateCommercialSegment;

namespace Api.Tests.BuilderCommands.CommercialSegmentCommands
{
    public class CreateCommercialSegmentCommandBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _name = "colchones";
        private string _description= "descripción de colchones";

        public CreateCommercialSegmentCommandBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }
        public CreateCommercialSegmentCommandBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public CreateCommercialSegmentCommandBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public CreateCommercialSegmentCommand Build()
        {
            return new CreateCommercialSegmentCommand(_id,_name,_description);
        }
    }
}