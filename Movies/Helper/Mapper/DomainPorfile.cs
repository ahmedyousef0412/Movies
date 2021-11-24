using AutoMapper;
using Movies.Models.Entity;
using Movies.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Helper.Mapper
{
    public class DomainPorfile  :Profile
    {
        public DomainPorfile()
        {
            CreateMap<Movie, MovieVM>();
            CreateMap<MovieVM, Movie>();



        }
    }
}
