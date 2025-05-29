namespace Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid CreatedBy { get; set; } = Guid.Empty;
        public DateTime? UpdatedAt { get; set; } = null;
        public DateTime? DeletedAt { get; set; } = null;
        public bool? IsDeleted { get; set; } = false;
    }
    // Ajustar store repository pra usar o createdby pra só poder editar/deletar o dono do store
}
