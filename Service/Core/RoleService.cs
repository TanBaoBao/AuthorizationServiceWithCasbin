using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Message;
using Data.Model.PaginationModel;
using Data.Models;
using Data.Utility.Paging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Data.Enums.EntityEnum;

namespace Service.Core
{
    public interface IRoleService
    {
        Task<ResultModel> AddRole(RoleCreateModel model);
        Task<ResultModel> GetRoles(PagingParam<RoleSortCriteria> paginationModel, RoleSearchModel searchModel);
        Task<ResultModel> GetRoleById(Guid id);
        Task<ResultModel> DeleteRole(Guid id);
        Task<ResultModel> UpdateRole(RoleUpdateModel model);
    }
    public class RoleService : IRoleService
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public RoleService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultModel> GetRoles(PagingParam<RoleSortCriteria> paginationModel, RoleSearchModel searchModel)
        {
            ResultModel result = new ResultModel();
            try
            {
                var roleQuery = _context.Role.Where(r => !r.IsDeleted);
                if (searchModel.Name != "")
                {
                    roleQuery = roleQuery.Where(t => t.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
                }
                if (searchModel.Domain != "")
                {
                    roleQuery = roleQuery.Where(t => t.Domain.ToUpper().Contains(searchModel.Domain.ToUpper()));
                }

                var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, roleQuery.Count());

                roleQuery = roleQuery.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
                roleQuery = roleQuery.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);

                var viewModels = await _mapper.ProjectTo<RoleViewModel>(roleQuery).ToListAsync();
                paging.Data = viewModels;
                result.IsSuccess = true;
                result.ResponseSuccess = paging;
            }
            catch (Exception e)
            {
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }
            return result;
        }

        public async Task<ResultModel> GetRoleById(Guid id)
        {
            ResultModel result = new ResultModel();
            try
            {
                var role = await _context.Role.Where(r => r.Id == id && !r.IsDeleted).FirstOrDefaultAsync();

                if (role == null)
                {
                    result.IsSuccess = false;
                    result.ResponseFailed = ErrorMessages.ID_NOT_EXIST;
                    result.Code = 404;
                    return result;
                }
                var model = _mapper.Map<Role, RoleViewModel>(role);
                result.IsSuccess = true;
                result.ResponseSuccess = model;
            }
            catch (Exception e)
            {
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }
            return result;
        }
        public async Task<ResultModel> AddRole(RoleCreateModel model)
        {
            ResultModel result = new ResultModel();
            try
            {
                var role = _mapper.Map<Role>(model);
                _context.Add(role);
                await _context.SaveChangesAsync();

                result.IsSuccess = true;
                result.ResponseSuccess = role.Id;
            }
            catch (Exception e)
            {
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }

            return result;
        }
        public async Task<ResultModel> UpdateRole(RoleUpdateModel model)
        {
            ResultModel result = new ResultModel();
            var role = await _context.Role.Where(r => r.Id == model.Id && !r.IsDeleted).FirstOrDefaultAsync();
            if (role == null)
            {

                result.IsSuccess = false;
                result.ResponseFailed = ErrorMessages.ID_NOT_EXIST;
                return result;
            }
            var transaction = _context.Database.BeginTransaction();
            try
            {
                role = _mapper.Map(model, role);
                role.DateUpdated = DateTime.Now;
                await _context.SaveChangesAsync();
                transaction.Commit();

                result.IsSuccess = true;
                result.ResponseSuccess = _mapper.Map<Role, RoleViewModel>(role);
            }
            catch (Exception e)
            {
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
                transaction.Rollback();
            }

            return result;

        }
        public async Task<ResultModel> DeleteRole(Guid id)
        {
            ResultModel result = new ResultModel();
            try
            {
                var role = await _context.Role.Where(r => r.Id == id && !r.IsDeleted).FirstOrDefaultAsync();
                if (role == null)
                {
                    result.IsSuccess = false;
                    result.ResponseFailed = ErrorMessages.ID_NOT_EXIST;
                    return result;
                }
                role.IsDeleted = true;
                role.DateUpdated = DateTime.Now;
                await _context.SaveChangesAsync();
                result.IsSuccess = true;
                result.ResponseSuccess = "Deleted successfully";
            }
            catch (Exception e)
            {
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }
            return result;
        }
    }
    }
