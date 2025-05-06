using ClickBytez.EF.Gateway.Core.Abstractions.Entities;

namespace ClickBytez.EF.DemoStore.Model
{
    public class Article : ExtendedEntity<Guid>
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? PublishedDate { get; set; }

        public Guid? UserId { get; set; }
        public virtual User? Author { get; set; }
    }
}
