using Application.UseCases.ComercialSegments.Commands.UpdateCommercialSegment;

namespace Api.Tests.BuilderCommands.CommercialSegmentCommands
{
    public class UpdateCommercialSegmentCommandBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _name = "colchones";
        private string _description = "descripción de colchones";

        public UpdateCommercialSegmentCommandBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }
        public UpdateCommercialSegmentCommandBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public UpdateCommercialSegmentCommandBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public UpdateCommercialSegmentCommand Build()
        {
            return new UpdateCommercialSegmentCommand(_id, _name, _description);
        }
    }
}