﻿using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Cabrones.Utils.Reflection
{
    public class TestTypeExtensions
    {
        [Fact]
        public void GetProperty_deve_funcionar_corretamente()
        {
            // Arrange, Given

            var propriedade = typeof(ClassePai).GetProperty("PropriedadeConcorrente");
            var métodoGetDePropriedade = propriedade?.GetMethod;
            var métodoSetDePropriedade = propriedade?.SetMethod;
            var métodoQueNãoÉDePropriedades = typeof(ClassePai).GetMethod("ClassePaiMétodoPúblicoEstático");
            
            // Act, When

            var detecçãoParaMétodoGetDePropriedade = métodoGetDePropriedade.GetProperty();
            var detecçãoParaMétodoSetDePropriedade = métodoSetDePropriedade.GetProperty();
            var detecçãoParaMétodoQueNãoÉDePropriedades = métodoQueNãoÉDePropriedades.GetProperty();
            
            // Assert, Then

            detecçãoParaMétodoGetDePropriedade.Should().BeSameAs(propriedade);
            detecçãoParaMétodoSetDePropriedade.Should().BeSameAs(propriedade);
            detecçãoParaMétodoQueNãoÉDePropriedades.Should().BeNull();
        }
        
        [Theory]
        [InlineData(typeof(IInterface1), 4, 4)]
        [InlineData(typeof(IInterface2), 4, 4)]
        [InlineData(typeof(IInterface3), 8, 4)]
        [InlineData(typeof(ClassePai), 16, 8)]
        [InlineData(typeof(ClasseFilha), 32, 20)]
        [InlineData(typeof(ClasseNeta), 40, 28)]
        public void AllProperties_deve_funcionar_corretamente(Type tipoParaTeste, int declaraçõesEsperadasComInterface, int declaraçõesEsperadasSemInterface)
        {
            // Arrange, Given

            var tipo = tipoParaTeste;

            // Act, When

            // ReSharper disable once RedundantArgumentDefaultValue
            var propriedadesSemInterfaces = tipo.AllProperties(false).Select(a => a.ToString()).ToList();
            var propriedadesComInterfaces = tipo.AllProperties(true).Select(a => a.ToString()).ToList();

            // Assert, Then

            propriedadesSemInterfaces.Should().HaveCount(declaraçõesEsperadasSemInterface);
            propriedadesComInterfaces.Should().HaveCount(declaraçõesEsperadasComInterface);
        }

        [Theory]
        [InlineData(typeof(IInterface1), 1, 1)]
        [InlineData(typeof(IInterface2), 1, 1)]
        [InlineData(typeof(IInterface3), 2, 1)]
        [InlineData(typeof(ClassePai), 18, 16)]
        [InlineData(typeof(ClasseFilha), 22, 19)]
        [InlineData(typeof(ClasseNeta), 23, 20)]
        public void AllMethods_deve_funcionar_corretamente(Type tipoParaTeste, int declaraçõesEsperadasComInterface, int declaraçõesEsperadasSemInterface)
        {
            // Arrange, Given

            var tipo = tipoParaTeste;

            // Act, When

            // ReSharper disable once RedundantArgumentDefaultValue
            var métodosSemInterfaces = tipo.AllMethods(false).Select(a => a.ToString()).ToList();
            var métodosComInterfaces = tipo.AllMethods(true).Select(a => a.ToString()).ToList();

            // Assert, Then

            métodosSemInterfaces.Should().HaveCount(declaraçõesEsperadasSemInterface);
            métodosComInterfaces.Should().HaveCount(declaraçõesEsperadasComInterface);
        }

        [Theory]
        [InlineData(typeof(IInterface1), true)]
        [InlineData(typeof(IInterface2), false, typeof(object))]
        [InlineData(typeof(ClasseNeta), true, typeof(ClasseFilha), typeof(ClassePai), typeof(IInterface3), typeof(IInterface2), typeof(IInterface1), typeof(object))]
        [InlineData(typeof(ClasseNeta), false, typeof(ClasseFilha), typeof(ClassePai), typeof(IInterface3), typeof(IInterface2), typeof(IInterface1))]
        [InlineData(typeof(ClasseNeta), false, typeof(ClasseFilha))]
        [InlineData(typeof(ClasseSozinha), true, typeof(object))]
        [InlineData(typeof(ClasseSozinha), false)]
        public void AllImplementations_deve_funcionar_corretamente(Type tipoParaTeste, bool estáCoreto, params Type[] implementaçõesEsperadas)
        {
            // Arrange, Given

            var tipo = tipoParaTeste;

            // Act, When

            var implementations = tipo.AllImplementations().ToList();

            // Assert, Then

            if (estáCoreto)
            {
                implementations.Should().BeEquivalentTo(implementaçõesEsperadas.ToList());
            }
            else
            {
                implementations.Should().NotBeEquivalentTo(implementaçõesEsperadas.ToList());
            }
        }
        
        [Theory]
        [InlineData(typeof(IInterface1), 4)]
        [InlineData(typeof(IInterface2), 4)]
        [InlineData(typeof(IInterface3), 4)]
        [InlineData(typeof(ClassePai), 8)]
        [InlineData(typeof(ClasseFilha), 12)]
        [InlineData(typeof(ClasseNeta), 8)]
        public void MyProperties_deve_funcionar_corretamente(Type tipoParaTeste, int declaraçõesEsperadas)
        {
            // Arrange, Given

            var tipo = tipoParaTeste;

            // Act, When

            var propriedades = tipo.MyProperties().Select(a => a.ToString()).ToList();

            // Assert, Then

            propriedades.Should().HaveCount(declaraçõesEsperadas);
        }

        [Theory]
        [InlineData(typeof(IInterface1), 1)]
        [InlineData(typeof(IInterface2), 1)]
        [InlineData(typeof(IInterface3), 1)]
        [InlineData(typeof(ClassePai), 8)]
        [InlineData(typeof(ClasseFilha), 3)]
        [InlineData(typeof(ClasseNeta), 1)]
        public void MyMethods_deve_funcionar_corretamente(Type tipoParaTeste, int declaraçõesEsperadas)
        {
            // Arrange, Given

            var tipo = tipoParaTeste;

            // Act, When

            var métodos = tipo.MyMethods().Select(a => a.ToString()).ToList();

            // Assert, Then

            métodos.Should().HaveCount(declaraçõesEsperadas);
        }

        [Theory]
        [InlineData(typeof(IInterface1), true)]
        [InlineData(typeof(IInterface2), false, typeof(object))]
        [InlineData(typeof(ClasseNeta), false, typeof(ClasseFilha), typeof(ClassePai), typeof(IInterface3), typeof(IInterface2), typeof(IInterface1), typeof(object))]
        [InlineData(typeof(ClasseNeta), true, typeof(ClasseFilha), typeof(ClassePai), typeof(IInterface3), typeof(IInterface2), typeof(IInterface1))]
        [InlineData(typeof(ClasseNeta), false, typeof(ClasseFilha))]
        [InlineData(typeof(ClasseSozinha), false, typeof(object))]
        [InlineData(typeof(ClasseSozinha), true)]
        public void MyImplementations_deve_funcionar_corretamente(Type tipoParaTeste, bool estáCoreto, params Type[] implementaçõesEsperadas)
        {
            // Arrange, Given

            var tipo = tipoParaTeste;

            // Act, When

            var implementations = tipo.MyImplementations().ToList();

            // Assert, Then

            if (estáCoreto)
            {
                implementations.Should().BeEquivalentTo(implementaçõesEsperadas.ToList());
            }
            else
            {
                implementations.Should().NotBeEquivalentTo(implementaçõesEsperadas.ToList());
            }
        }

        [Theory]
        [InlineData(typeof(IInterface1), 4)]
        [InlineData(typeof(IInterface2), 4)]
        [InlineData(typeof(IInterface3), 4)]
        [InlineData(typeof(ClassePai), 0)]
        [InlineData(typeof(ClasseFilha), 8)]
        [InlineData(typeof(ClasseNeta), 8)]
        public void MyOwnProperties_deve_funcionar_corretamente(Type tipoParaTeste, int declaraçõesEsperadas)
        {
            // Arrange, Given

            var tipo = tipoParaTeste;

            // Act, When

            var métodos = tipo.MyOwnProperties().Select(a => a.ToString()).ToList();

            // Assert, Then

            métodos.Should().HaveCount(declaraçõesEsperadas);
        }

        [Theory]
        [InlineData(typeof(IInterface1), 1)]
        [InlineData(typeof(IInterface2), 1)]
        [InlineData(typeof(IInterface3), 1)]
        [InlineData(typeof(ClassePai), 6)]
        [InlineData(typeof(ClasseFilha), 0)]
        [InlineData(typeof(ClasseNeta), 1)]
        public void MyOwnMethods_deve_funcionar_corretamente(Type tipoParaTeste, int declaraçõesEsperadas)
        {
            // Arrange, Given

            var tipo = tipoParaTeste;

            // Act, When

            var métodos = tipo.MyOwnMethods().Select(a => a.ToString()).ToList();

            // Assert, Then

            métodos.Should().HaveCount(declaraçõesEsperadas);
        }

        [Theory]
        [InlineData(typeof(IInterface1), true)]
        [InlineData(typeof(IInterface2), false, typeof(object))]
        [InlineData(typeof(ClasseNeta), false, typeof(ClasseFilha), typeof(ClassePai), typeof(IInterface3), typeof(IInterface2), typeof(IInterface1), typeof(object))]
        [InlineData(typeof(ClasseNeta), false, typeof(ClasseFilha), typeof(ClassePai), typeof(IInterface3), typeof(IInterface2), typeof(IInterface1))]
        [InlineData(typeof(ClasseNeta), true, typeof(ClasseFilha))]
        [InlineData(typeof(ClasseSozinha), false, typeof(object))]
        [InlineData(typeof(ClasseSozinha), true)]
        public void MyOwnImplementations_deve_funcionar_corretamente(Type tipoParaTeste, bool estáCoreto, params Type[] implementaçõesEsperadas)
        {
            // Arrange, Given

            var tipo = tipoParaTeste;

            // Act, When

            var implementations = tipo.MyOwnImplementations().ToList();

            // Assert, Then

            if (estáCoreto)
            {
                implementations.Should().BeEquivalentTo(implementaçõesEsperadas.ToList());
            }
            else
            {
                implementations.Should().NotBeEquivalentTo(implementaçõesEsperadas.ToList());
            }
        }
    }
}