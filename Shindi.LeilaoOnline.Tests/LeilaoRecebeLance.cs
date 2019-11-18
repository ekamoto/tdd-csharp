﻿using Xunit;
using Shindi.LeilaoOnline.Core;
using System.Linq;

namespace Shindi.LeilaoOnline.Tests
{
    public class LeilaoRecebeLance
    {
        [Theory]
        [InlineData(1, new double[] { 100, 200})]
        [InlineData(1, new double[] { 100, 200, 635, 985 })]
        public void NaoPermiteNovosLancesDadoLeilaoFinalizado(int qtdEsperada, double[] ofertas)
        {
            // Arranje - cenário
            var leilao = new Leilao("Bicicleta");
            leilao.IniciaPregao();

            var interassado1 = new Interessada("Leandro", leilao);
            var interassado2 = new Interessada("Priscila", leilao);

            foreach (var oferta in ofertas)
            {
                leilao.RecebeLance(interassado1, oferta);
            }

            leilao.TerminaPregao();

            // Act - método sob teste
            leilao.RecebeLance(interassado2, 532);
            var quantidadeDeLancesEsperado = qtdEsperada;

            Assert.Equal(leilao.Lances.Count(), quantidadeDeLancesEsperado);
            Assert.Equal(100, leilao.Lances.First().Valor);
        }

        [Theory]
        [InlineData(0, new double[] { 100, 200 })]
        [InlineData(0, new double[] { 100, 200, 635, 985 })]
        public void QtdePermaneceZeroDadoQuePregaoNaoFoiIniciado(int qtdEsperada, double[] ofertas) 
        {
            // Arranje - cenário
            var leilao = new Leilao("Bicicleta");
            
            var interassado1 = new Interessada("Leandro", leilao);
            var interassado2 = new Interessada("Priscila", leilao);

            foreach (var oferta in ofertas)
            {
                leilao.RecebeLance(interassado1, oferta);
            }

            var quantidadeDeLancesEsperado = qtdEsperada;

            Assert.Equal(leilao.Lances.Count(), quantidadeDeLancesEsperado);
        }

        [Fact]
        public void NaoPermiteNovosLancesDadoMesmoInteressado()
        {
            var leilao = new Leilao("Bicicleta");

            var interassado1 = new Interessada("Leandro", leilao);

            leilao.IniciaPregao();
            leilao.RecebeLance(interassado1, 200);
            leilao.RecebeLance(interassado1, 300);

            double valorEsperado = 200;
            int qtdEsperado = 1;
            Assert.Equal(valorEsperado, leilao.Lances.FirstOrDefault().Valor);
            Assert.Equal(qtdEsperado, leilao.Lances.Count());
        }
    }
}