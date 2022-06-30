using AutoFixture;
using RapidAuto.MVC.Interfaces;
using RapidAuto.MVC.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using RapidAuto.MVC.Models.Utilisateurs;
using System.Net.Http;

namespace RapidAuto.MVC.UnitTests
{
    public class GestionUtilisateursControllerTests
    {
        private readonly Mock<IGestionUtilisateursService> _mockUtilisateurService;
        private readonly ILogger<GestionUtilisateursController> _mockLogger;
        private readonly Fixture _fixture;

        public GestionUtilisateursControllerTests()
        {
            _mockUtilisateurService = new Mock<IGestionUtilisateursService>();
            _mockLogger = Mock.Of<ILogger<GestionUtilisateursController>>();
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Index_Retourne_ViewResult()
        {
            //Etant donné
            _fixture.RepeatCount = 5;
            var utilisateurs = _fixture.Create<List<Utilisateur>>();

            _mockUtilisateurService.Setup(e => e.ObtenirTout()).Returns(() => Task.FromResult(utilisateurs));
            var utilisateursController = new GestionUtilisateursController(_mockUtilisateurService.Object, _mockLogger);

            //Quand
            var viewResult = await utilisateursController.Index() as ViewResult;

            //Alors
            viewResult.Should().NotBeNull();
            var utilisateursResult = viewResult.Model as IEnumerable<Utilisateur>;
            utilisateursResult.Should().BeEquivalentTo(utilisateurs);
        }

        [Fact]
        public async Task Details_IdValide_Retourne_ViewResult()
        {
            //Etant donné
            var utilisateur = _fixture.Create<Utilisateur>();
            int id = 1;

            _mockUtilisateurService.Setup(e => e.ObtenirParId(It.IsNotNull<int>())).Returns(() => Task.FromResult(utilisateur));
            var utilisateursController = new GestionUtilisateursController(_mockUtilisateurService.Object, _mockLogger);

            //Quand
            var viewResult = await utilisateursController.Details(id) as ViewResult;

            //Alors
            viewResult.Should().NotBeNull();
            var utilisateurResult = viewResult.Model as Utilisateur;
            utilisateurResult.Should().Be(utilisateur);
        }

        [Fact]
        public void HttpGet_Create_Retourne_View()
        {
            var utilisateursController = new GestionUtilisateursController(_mockUtilisateurService.Object, _mockLogger);

            //Quand
            var viewResult = utilisateursController.Create() as ViewResult;

            //Alors
            viewResult.Should().NotBeNull();
        }

            [Fact]
        public async Task HttpPost_Create_ModeleValide_Retourne_RedirectToAction()
        {
            //Etant donné
            var utilisateur = _fixture.Create<Utilisateur>();
            var utilisateursController = new GestionUtilisateursController(_mockUtilisateurService.Object, _mockLogger);

            var httpResponseMessage = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.Created };

            _mockUtilisateurService.Setup(e => e.Ajouter(It.IsAny<Utilisateur>())).Returns(() => Task.FromResult(httpResponseMessage));

            //Quand
            var redirectToActionResult = await utilisateursController.Create(utilisateur) as RedirectToActionResult;

            //Alors
            redirectToActionResult.Should().NotBeNull();
            redirectToActionResult.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task HttpPost_Create_ModeleInvalide_Retourne_ViewAvecModel()
        {
            //Etant donné
            var utilisateur = _fixture.Create<Utilisateur>();
            var utilisateursController = new GestionUtilisateursController(_mockUtilisateurService.Object, _mockLogger);
            utilisateursController.ModelState.AddModelError("test", "test");

            //Quand
            var viewResult = await utilisateursController.Create(utilisateur) as ViewResult;

            //Alors
            viewResult.Should().NotBeNull();
            utilisateursController.ViewData.Should().NotBeNull();
            var utilisateurResult = viewResult.Model as Utilisateur;
            utilisateurResult.Should().Be(utilisateur);
        }

        [Fact]
        public async Task HttpPost_Create_ErreurHttp_Retourne_ViewAvecModel_Et_ModelStateError()
        {
            //Etant donné
            var utilisateur = _fixture.Create<Utilisateur>();
            var utilisateursController = new GestionUtilisateursController(_mockUtilisateurService.Object, _mockLogger);

            var httpResponseMessage = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest};

            _mockUtilisateurService.Setup(e => e.Ajouter(It.IsAny<Utilisateur>())).Returns(() => Task.FromResult(httpResponseMessage));

            //Quand
            var viewResult = await utilisateursController.Create(utilisateur) as ViewResult;

            //Alors
            viewResult.Should().NotBeNull();
            var utilisateurResult = viewResult.Model as Utilisateur;
            utilisateurResult.Should().Be(utilisateur);
            viewResult.ViewData.ModelState["Erreur du service"].Errors.FirstOrDefault().ErrorMessage.Should().Be(await httpResponseMessage.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task HttpGet_Edit_Retourne_View()
        {
            //Etant donné
            var utilisateur = _fixture.Create<Utilisateur>();
            int id = 1;

            _mockUtilisateurService.Setup(e => e.ObtenirParId(It.IsNotNull<int>())).Returns(() => Task.FromResult(utilisateur));
            var utilisateursController = new GestionUtilisateursController(_mockUtilisateurService.Object, _mockLogger);

            //Quand
            var viewResult = await utilisateursController.Edit(id) as ViewResult;

            //Alors
            viewResult.Should().NotBeNull();
            var utilisateurResult = viewResult.Model as Utilisateur;
            utilisateurResult.Should().Be(utilisateur);
        }

        [Fact]
        public async Task HttpGet_Edit_Exception_Retourne_View()
        {
            //Etant donné
            var exception = new HttpRequestException();

            _mockUtilisateurService.Setup(e => e.ObtenirParId(It.IsNotNull<int>())).Throws(exception);
            var utilisateursController = new GestionUtilisateursController(_mockUtilisateurService.Object, _mockLogger);

            //Quand
            var viewResult = await utilisateursController.Edit(1) as ViewResult;

            //Alors
            viewResult.Should().NotBeNull();
            var utilisateurResult = viewResult.Model as Utilisateur;
        }

        [Fact]
        public async Task HttpPost_Edit_ModeleValide_IdDifferentDuModel_Retourne_NotFound()
        {
            //Etant donné
            var utilisateur = _fixture.Create<Utilisateur>();

            var utilisateursController = new GestionUtilisateursController(_mockUtilisateurService.Object, _mockLogger);

            //Quand
            var redirectToActionResult = await utilisateursController.Edit(-1, utilisateur) as NotFoundResult;

            //Alors
            redirectToActionResult.Should().NotBeNull();
        }

        [Fact]
        public async Task HttpPost_Edit_ModeleValide_Et_HttpStatusCode_Valide_Retourne_RedirectToAction()
        {
            //Etant donné
            var utilisateur = _fixture.Create<Utilisateur>();

            _mockUtilisateurService.Setup(e => e.Modifier(utilisateur.ID, utilisateur)).Returns(() => Task.FromResult(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.Created }));
            var utilisateursController = new GestionUtilisateursController(_mockUtilisateurService.Object, _mockLogger);

            //Quand
            var redirectToActionResult = await utilisateursController.Edit(utilisateur.ID,utilisateur) as RedirectToActionResult;

            //Alors
            redirectToActionResult.Should().NotBeNull();
            redirectToActionResult.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task HttpPost_Edit_ModeleValide_Et_HttpStatusCode_Invalide_Retourne_RedirectToAction()
        {
            //Etant donné
            var utilisateur = _fixture.Create<Utilisateur>();

            _mockUtilisateurService.Setup(e => e.Modifier(utilisateur.ID, utilisateur)).Returns(() => Task.FromResult(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.Unauthorized }));
            var utilisateursController = new GestionUtilisateursController(_mockUtilisateurService.Object, _mockLogger);

            //Quand
            var viewResult = await utilisateursController.Edit(utilisateur.ID, utilisateur) as ViewResult;

            //Alors
            viewResult.Should().NotBeNull();
            
            utilisateursController.ViewData.Should().NotBeNull();
            var utilisateurResult = viewResult.Model as Utilisateur;
            utilisateurResult.Should().Be(utilisateur);

        }
        
        [Fact]
        public async Task HttpPost_Edit_ModeleInvalide_Retourne_ViewUtilisateur()
        {
            //Etant donné
            var utilisateur = _fixture.Create<Utilisateur>();
            var utilisateursController = new GestionUtilisateursController(_mockUtilisateurService.Object, _mockLogger);
            utilisateursController.ModelState.AddModelError("test", "test");

            //Quand
            var viewResult = await utilisateursController.Edit(utilisateur.ID, utilisateur) as ViewResult;

            //Alors
            viewResult.Should().NotBeNull();
            utilisateursController.ViewData.Should().NotBeNull();
            var utilisateurResult = viewResult.Model as Utilisateur;
            utilisateurResult.Should().Be(utilisateur);
        }

        [Fact]
        public async Task HttpGet_Delete_Retourne_View()
        {
            //Etant donné
            var utilisateur = _fixture.Create<Utilisateur>();
            int id = 1;

            _mockUtilisateurService.Setup(e => e.ObtenirParId(It.IsNotNull<int>())).Returns(() => Task.FromResult(utilisateur));
            var utilisateursController = new GestionUtilisateursController(_mockUtilisateurService.Object, _mockLogger);

            //Quand
            var viewResult = await utilisateursController.Delete(id) as ViewResult;

            //Alors
            viewResult.Should().NotBeNull();
            var utilisateurResult = viewResult.Model as Utilisateur;
            utilisateurResult.Should().Be(utilisateur);
        }
        [Fact]
        public async Task HttpGet_Delete_IdNull_Retourne_View()
        {
            //Etant donné
            Utilisateur utilisateur = null;

            _mockUtilisateurService.Setup(e => e.ObtenirParId(It.IsNotNull<int>())).Returns(() => Task.FromResult(utilisateur));
            var utilisateursController = new GestionUtilisateursController(_mockUtilisateurService.Object, _mockLogger);

            //Quand
            var actionResult = await utilisateursController.Delete(null) as NotFoundResult;

            //Alors
            actionResult.Should().NotBeNull();
        }
        [Fact]
        public async Task HttpPost_Delete_ModeleValide_Retourne_RedirectToAction()
        {
            //Etant donné
            var utilisateur = _fixture.Create<Utilisateur>();
            _mockUtilisateurService.Setup(e => e.ObtenirParId(It.IsNotNull<int>())).Returns(() => Task.FromResult(utilisateur));
            var utilisateursController = new GestionUtilisateursController(_mockUtilisateurService.Object, _mockLogger);

            //Quand
            var redirectToActionResult = await utilisateursController.Delete(utilisateur.ID, utilisateur) as RedirectToActionResult;

            //Alors
            redirectToActionResult.Should().NotBeNull();
            redirectToActionResult.ActionName.Should().Be("Index");
        }
        [Fact]
        public async Task HttpPost_Delete_Exception_Retourne_Retourne_View()
        {
            //Etant donné
            var utilisateur = _fixture.Create<Utilisateur>();
            var exception = new HttpRequestException();

            _mockUtilisateurService.Setup(e => e.Supprimer(It.IsNotNull<int>())).Throws(exception);
            var utilisateursController = new GestionUtilisateursController(_mockUtilisateurService.Object, _mockLogger);

            //Quand
            var result = await utilisateursController.Delete(utilisateur.ID, utilisateur) as ViewResult;

            result.Should().NotBeNull();
            var utilisateurResult = result.Model as Utilisateur;
            utilisateurResult.Should().Be(utilisateur);
        }
        [Fact]
        public async Task HttpPost_Delete_UtilisateurIntrouvable_Retourne_View()
        {
            //Etant donné
            var utilisateur = _fixture.Create<Utilisateur>();
            var utilisateursController = new GestionUtilisateursController(_mockUtilisateurService.Object, _mockLogger);

            //Quand
            var viewResult = await utilisateursController.Delete(-1, utilisateur) as ViewResult;

            //Alors
            viewResult.Should().BeNull();
            utilisateursController.ViewData.Should().NotBeNull();
            _mockUtilisateurService.Verify(x => x.Supprimer(It.IsAny<int>()), Times.Never); 
        }
    }
    
}

