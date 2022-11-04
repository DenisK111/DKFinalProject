﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metflix.DL.Repositories.Contracts;
using Metflix.Host.DL.Seeding.Contracts;
using Metflix.Models.DbModels;
using Newtonsoft.Json;

namespace Metflix.Host.DL.Seeding.Implementations
{
    public class MoviesSeeder : ISeeder
    {
        private readonly IMovieRepository _movieRepository;

        public MoviesSeeder(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task SeedAsync()
        {
            if ((await _movieRepository.GetAll()).Any())
            {
                return;
            }

            var data = await File.ReadAllTextAsync("bin/Debug/net6.0/Seeding/SeedFiles/MoviesSeed.json");
            var movies = JsonConvert.DeserializeObject<IEnumerable<Movie>>(data);

            foreach (var movie in movies!)
            {
                await _movieRepository.Add(movie);
            }
        }
    }
}
