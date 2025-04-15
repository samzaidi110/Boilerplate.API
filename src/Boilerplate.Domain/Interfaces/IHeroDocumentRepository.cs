using Boilerplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Domain.Interfaces;
public interface IHeroDocumentRepository
{
    Hero Save(Hero request);
}
