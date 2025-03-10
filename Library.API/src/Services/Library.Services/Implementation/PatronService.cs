using Library.Data.Models;
using Library.Services.DTOs;
using Library.Services.Infrastructure;
using Library.Services.Interfaces;
using Library.SharedKernal.OperationResults;
using Mapster;
using System.Net;

namespace Library.Services.Implementation
{
    public class PatronService(IUnitOfWork unitOfWork) : IPatronService
    {

        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<OperationResult<HttpStatusCode, PatronResponseDto>> GetByIdAsync(int id)
        {
            var result = new OperationResult<HttpStatusCode, PatronResponseDto>();
            if (id <= 0)
            {
                result.AddError("Id should be greater than 0");
                result.EnumResult = HttpStatusCode.BadRequest;
                return result;
            }
            var patronEntity = await _unitOfWork.Patrons.GetByIdAsync(id);

            if (patronEntity == null)
            {
                result.AddError("Patron Not Found");
                result.EnumResult = HttpStatusCode.NotFound;
                return result;
            }

            var patron = patronEntity.Adapt<PatronResponseDto>();

            result.Result = patron;
            result.EnumResult = HttpStatusCode.OK;

            return result;

        }

        public OperationResult<HttpStatusCode, IEnumerable<PatronResponseDto>> GetAll()
        {
            var result = new OperationResult<HttpStatusCode, IEnumerable<PatronResponseDto>>();

            var patrons = _unitOfWork.Patrons.GetTableNoTracking().Select(x => new PatronResponseDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                DateOfBirth = x.DateOfBirth,
                MembershipStartDate = x.MembershipStartDate,
                MembershipEndDate = x.MembershipEndDate,
                Address = x.Address,
                Email = x.Email,
                IsActive = x.IsActive,
                PhoneNumber = x.PhoneNumber,

            });

            result.Result = patrons;
            result.EnumResult = HttpStatusCode.OK;
            return result;
        }

        public async Task<OperationResult<HttpStatusCode, bool>> AddAsync(PatronDto patron)
        {
            var result = new OperationResult<HttpStatusCode, bool>();

            if (patron == null)
            {
                result.EnumResult = HttpStatusCode.InternalServerError;
                result.AddError("Patron can not be null!");
                return result;
            }
         
            var patronEntitiy = patron.Adapt<Patron>();

            await _unitOfWork.Patrons.AddAsync(patronEntitiy);

            await _unitOfWork.CommitAsync();

            result.EnumResult = HttpStatusCode.OK;
            result.Result = true;
            return result;
        }

        public async Task<OperationResult<HttpStatusCode, bool>> UpdateAsync(int id, PatronDto patron)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
            if (id <= 0)
            {
                result.AddError("Id should be greater than 0");
                result.EnumResult = HttpStatusCode.BadRequest;
                return result;
            }

            var patronEntity = await _unitOfWork.Patrons.GetByIdAsync(id);

            if (patronEntity == null)
            {
                result.AddError("Patron Not Found");
                result.EnumResult = HttpStatusCode.NotFound;
                return result;
            }

            patronEntity.Address = patron.Address;
            patronEntity.PhoneNumber = patron.PhoneNumber;
            patronEntity.DateOfBirth = patron.DateOfBirth;
            patronEntity.Email = patron.Email;
            patronEntity.MembershipStartDate = patron.MembershipStartDate;
            patronEntity.MembershipEndDate = patron.MembershipEndDate;
            patronEntity.FirstName = patron.FirstName;
            patronEntity.LastName = patron.LastName;
            patronEntity.IsActive = patron.IsActive;
            
            await _unitOfWork.Patrons.UpdateAsync(patronEntity);
                        await _unitOfWork.CommitAsync();


            result.EnumResult = HttpStatusCode.OK;
            result.Result = true;

            return result;

        }

        public async Task<OperationResult<HttpStatusCode, bool>> DeleteAsync(int id)
        {
            var result = new OperationResult<HttpStatusCode, bool>();

            if (id <= 0)
            {
                result.AddError("Id should be greater than 0");
                result.EnumResult = HttpStatusCode.BadRequest;
                return result;
            }
            var patronEntity = await _unitOfWork.Patrons.GetByIdAsync(id);


            if (patronEntity == null)
            {
                result.AddError("Patron Not Found");
                result.EnumResult = HttpStatusCode.NotFound;
                return result;
            }

            await _unitOfWork.Patrons.DeleteAsync(patronEntity);
            await _unitOfWork.CommitAsync();

            result.Result = true;
            result.EnumResult = HttpStatusCode.OK;

            return result;

        }

      


    }



}
