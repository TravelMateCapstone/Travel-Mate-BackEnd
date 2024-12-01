﻿using BusinessObjects;
using BusinessObjects.Entities;
using BusinessObjects.EnumClass;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace DataAccess
{
    public class TourDAO
    {
        private readonly ApplicationDBContext _sqlContext;
        //private readonly MongoDbContext _mongoContext;
        private readonly IMongoCollection<Tour> _mongoContext;


        public TourDAO(ApplicationDBContext context, MongoDbContext mongoContext)
        {
            _sqlContext = context;
            _mongoContext = mongoContext.GetCollection<Tour>("Tours");
        }

        public async Task<ApplicationUser> GetLocalInfor(int userId)
        {
            return _sqlContext.Users
                .Include(t => t.Profiles)
                .Include(t => t.PastTripPosts.Where(p => p.LocalId == userId))
                .FirstOrDefault(t => t.Id == userId);
        }

        //get list participants of a tour

        //get list tour of a participants

        //get list tour of local based on approval status
        public IEnumerable<Tour> GetAllToursOfLocal(int userId)
        {
            return _mongoContext.Find(t => t.Creator.Id == userId).ToList();
        }

        public async Task<IEnumerable<Tour>> GetToursByStatus(int userId, ApprovalStatus? approvalStatus)
        {
            return _mongoContext.Find(t => t.Creator.Id == userId && t.ApprovalStatus == approvalStatus).ToList();
        }

        // Get all tours
        public IEnumerable<Tour> GetAllTours()
        {
            return _mongoContext.Find(_ => true).ToList();
        }

        // Get a tour by ID
        public async Task<Tour> GetTourById(string id)
        {
            return _mongoContext.Find(t => t.TourId == id).FirstOrDefault();
        }

        // Add a new tour
        public async Task AddTour(Tour tour)
        {
            _mongoContext.InsertOne(tour);
        }

        // Update an existing tour
        public async Task UpdateTour(string id, Tour tour)
        {
            var filter = Builders<Tour>.Filter.Eq(f => f.TourId, id);
            _mongoContext.ReplaceOne(filter, tour);
        }

        // Delete a tour
        public async Task DeleteTour(string id)
        {
            var filter = Builders<Tour>.Filter.Eq(f => f.TourId, id);
            _mongoContext.DeleteOne(filter);
        }

        public async Task JoinTour(string tourId, Participants participant)
        {

            // Tạo bộ lọc để tìm tour theo tourId
            var filter = Builders<Tour>.Filter.Eq(f => f.TourId, tourId);

            // Tạo update để thêm travelerId vào mảng participants
            var update = Builders<Tour>.Update.Push(f => f.Participants, participant);

            // Thực hiện cập nhật tour với toán tử $push để thêm travelerId vào participants
            await _mongoContext.UpdateOneAsync(filter, update);
        }

        // Admin accepts a tour
        public async Task ProcessTourAdmin(string tourId, ApprovalStatus processStatus)
        {
            var filter = Builders<Tour>.Filter.Eq(f => f.TourId, tourId);

            var update = Builders<Tour>.Update.Set(t => t.ApprovalStatus, processStatus);
            await _mongoContext.UpdateOneAsync(filter, update);
        }

        // Add a review to a tour
        public async Task AddReview(string tourId, TourReview tourReview)
        {
            var filter = Builders<Tour>.Filter.Eq(f => f.TourId, tourId);

            var update = Builders<Tour>.Update.AddToSet(t => t.Reviews, tourReview);
            await _mongoContext.UpdateOneAsync(filter, update);
        }

        // Update availability of a tour
        public async Task UpdateAvailability(string tourId, int slots)
        {
            var filter = Builders<Tour>.Filter.Eq(f => f.TourId, tourId);

            var update = Builders<Tour>.Update.Set(t => t.RegisteredGuests, slots);
            await _mongoContext.UpdateOneAsync(filter, update);
        }

        // Cancel a tour
        public async Task CancelTour(string tourId)
        {
            var filter = Builders<Tour>.Filter.Eq(f => f.TourId, tourId);

            var update = Builders<Tour>.Update.Set(t => t.TourStatus, false);
            await _mongoContext.UpdateOneAsync(filter, update);
        }

        public async Task<bool> DoesParticipantExist(int userId)
        {

            // Lọc để tìm các tour có participant với Id khớp userId
            var filter = Builders<Tour>.Filter.ElemMatch(
                t => t.Participants,
                p => p.participantId == userId
            );

            // Kiểm tra xem có tài liệu nào thỏa mãn filter hay không
            var count = await _mongoContext.CountDocumentsAsync(filter);
            return count > 0;
        }

    }
}