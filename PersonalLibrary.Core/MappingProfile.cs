using AutoMapper;
using PersonalLibrary.Core.Models;
using PersonalLibrary.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalLibrary.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {

            CreateMap<Book, GetBookDTO>()
                .ForMember(dest => dest.AuthorNames, opt => opt
                .MapFrom(src => src.BookAuthors.Select(ba => ba.Author.AuthorName).ToList()));

            //map AuthorNames(AddBookDTO) FROM BA collection in Book(which also has same prop)
            CreateMap<Book, AddBookDTO>()
                .ForMember(dest => dest.AuthorNames, opt => opt
                .MapFrom(src => src.BookAuthors.Select(ba => ba.Author.AuthorName).ToList()));

            // Ignore BookAuthors as it needs custom handling
            CreateMap<AddBookDTO, Book>()
            .ForMember(dest => dest.BookAuthors, opt => opt.Ignore());

            CreateMap<Book, UpdateBookDTO>()
                .ForMember(dest => dest.AuthorNames, opt => opt
                .MapFrom(src => src.BookAuthors.Select(ba => ba.Author.AuthorName).ToList()));

            CreateMap<UpdateBookDTO, Book>()
                .ForMember(dest => dest.BookAuthors, opt => opt.Ignore());
        }
    }
}
