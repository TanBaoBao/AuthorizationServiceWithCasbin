using AutoMapper;
using Data.Entities;
using Data.Models;

namespace Services.Mapping
{
    public class MapperProfile :Profile
    {
        public MapperProfile()
        {
            #region Role
            CreateMap<RoleModel, Role>().ReverseMap();
            CreateMap<Role, RoleViewModel>();
            CreateMap<RoleUpdateModel, Role>().ForAllMembers(opt => opt.Condition((src, des, srcMember) => srcMember != null));
            #endregion

        }
    }
}
