using System;
using RVA.Shared.Models;
using System.Collections.Generic;
using System.Linq;

using Komponenta1.InformacioniSistem.Interfaces;
namespace Komponenta1.InformacioniSistem.Services
{
	public class BiciklService : IBiciklService
	{
        private readonly DataStore storage;
        private readonly ILogger logger;

        public BiciklService(DataStore storage, ILogger logger)
        {
            this.storage = storage;
            this.logger = logger;
        }

        public List<TrkackiBicikl> GetAll()
        {
            return storage.Bicikli.ToList();
        }

        public List<TrkackiBicikl> Search(Guid? id, string tim, string vozac, double? tezina, bool? sprinter)
        {
            return storage.Bicikli
                .Where(b =>
                    (!id.HasValue || b.Id == id.Value) &&
                    (string.IsNullOrWhiteSpace(tim) || b.Tim.ToLower().Contains(tim.ToLower())) &&
                    (string.IsNullOrWhiteSpace(vozac) || b.Vozac.ToLower().Contains(vozac.ToLower())) &&
                    (!tezina.HasValue || tezina.Value <= 0 || b.Tezina == tezina.Value) &&
                    (!sprinter.HasValue || b.Sprinter == sprinter.Value))
                .ToList();
        }

        public void Add(TrkackiBicikl bicikl)
        {
            if (bicikl == null)
            {
                throw new ArgumentNullException(nameof(bicikl));
            }

            if (bicikl.Id == Guid.Empty)
            {
                bicikl.Id = Guid.NewGuid();
            }

            storage.Bicikli.Add(bicikl);
            logger.Log($"Dodat trkacki bicikl: {bicikl}");
        }

        public void Update(TrkackiBicikl bicikl)
        {
            if (bicikl == null)
            {
                throw new ArgumentNullException(nameof(bicikl));
            }

            TrkackiBicikl existing = storage.Bicikli.FirstOrDefault(b => b.Id == bicikl.Id);

            if (existing == null)
            {
                throw new InvalidOperationException("Trkacki bicikl nije pronadjen.");
            }

            existing.Tim = bicikl.Tim;
            existing.Vozac = bicikl.Vozac;
            existing.Tezina = bicikl.Tezina;
            existing.Sprinter = bicikl.Sprinter;

            logger.Log($"Izmijenjen trkacki bicikl: {existing}");
        }

        public void Delete(Guid id)
        {
            TrkackiBicikl existing = storage.Bicikli.FirstOrDefault(b => b.Id == id);

            if (existing == null)
            {
                throw new InvalidOperationException("Trkacki bicikl nije pronadjen.");
            }

            storage.Bicikli.Remove(existing);
            logger.Log($"Obrisan trkacki bicikl: {existing}");
        }

        
    }
}
