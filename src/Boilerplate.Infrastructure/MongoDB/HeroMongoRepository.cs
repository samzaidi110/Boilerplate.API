using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Interfaces;

using Mapster;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Infrastructure.MongoDB;
public class HeroMongoRepository : IHeroDocumentRepository

{

    private readonly IMongoCollection<Hero> _collection;

    public HeroMongoRepository(MongoDBConnection dbConnection)
    {
        _collection = dbConnection.GetCollection<Hero>("Heroes");
    }

    public Hero Save(Hero request)
    {
        _collection.InsertOne(request);
        return request.Adapt<Hero>();
    }



}