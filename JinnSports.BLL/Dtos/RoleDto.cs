using Microsoft.AspNet.Identity;
using System;

namespace JinnSports.BLL.Dtos
{
    public class RoleDto : IRole<Guid>
    {
        public RoleDto()
        {
            this.Id = Guid.NewGuid();
        }

        public RoleDto(string name)
            : this()
        {
            this.Name = name;
        }

        public RoleDto(string name, Guid id)
        {
            this.Name = name;
            this.Id = id;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
