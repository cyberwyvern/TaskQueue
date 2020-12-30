using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using TaskQueue.API.Models;
using TaskQueue.API.Util;
using TaskQueue.DAO.Entities;
using TaskQueue.DAO.Interface;
using TaskQueue.API.DTO;
using TaskQueue.API.Services;

namespace TaskQueue.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class TaskController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDatabaseUnitOfWork _db;
        private readonly TaskQueuePublisherService _taskQueuePublisher;

        public TaskController(IMapper mapper, IDatabaseUnitOfWork db, TaskQueuePublisherService taskQueuePublisher)
        {
            _mapper = mapper;
            _db = db;
            _taskQueuePublisher = taskQueuePublisher;
        }

        [HttpPost]
        public IActionResult Post(CreateTaskModel model)
        {
            var userProfileEntity = GetOrCreateProfile();
            var taskEntity = new TaskEntity
            {
                UserProfileId = userProfileEntity.Id,
                Title = model.Title,
                CreatedDate = DateTime.UtcNow,
                Status = TaskStatus.Created
            };

            try
            {
                _db.StartTransaction();
                _db.Tasks.Create(taskEntity);

                TaskEntityDTO taskEntityDTO = _mapper.Map<TaskEntityDTO>(taskEntity);
                _taskQueuePublisher.Publish(taskEntityDTO);

                _db.Commit();

                return Ok();
            }
            catch
            {
                _db.Rollback();
                throw;
            }
        }

        [HttpGet("page")]
        public IActionResult GetPage([FromQuery] PageRequest request)
        {
            var page = _db.Tasks.GetUserTasksPage(request.PageIndex, request.PageSize, User.GetId());
            var resultPage = page.Convert(i => _mapper.Map<TaskModel>(i));
            return Ok(resultPage);
        }

        private UserProfile GetOrCreateProfile()
        {
            var userProfileEntity = _db.UserProfiles.GetByUserId(User.GetId());

            if (userProfileEntity == null)
            {
                try
                {
                    _db.StartTransaction();
                    userProfileEntity = new UserProfile { UserId = User.GetId(), Username = User.Identity.Name };
                    _db.UserProfiles.Create(userProfileEntity);
                    _db.Commit();
                }
                catch
                {
                    _db.Rollback();
                    throw;
                }
            }

            return userProfileEntity;
        }
    }
}