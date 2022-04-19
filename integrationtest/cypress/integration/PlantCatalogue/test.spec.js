// test.spec.js created with Cypress
//
// Start writing your Cypress tests below!
// If you're unfamiliar with how Cypress works,
// check out the link below and learn how to write your first test:


// https://on.cypress.io/writing-first-test
describe('Webapp teszt',()=>{

    it('Belépés',()=>{
        //Belépés
        cy.visit('https://localhost:44377/');
        cy.get('form.d-flex > [type="email"]').type('ark.alex@simonyiszki.org');
        cy.get('form.d-flex > [type="password"]').type('asd');
        cy.get('[value="Bejelentkezés"]').click();
    })

    it('Kártyák',()=>{
        //Kártyára kattintás
        cy.get(':nth-child(1) > .card > .button > .card-img-top').click();

        //Komment
        cy.get('[name="Comm"]').type('Jó ötletek');
        cy.get('.comment-move > [type="submit"]').click();
        cy.get('.btn-warning').click();
    })

    it('Kereső',()=>{
         //Keresés
         cy.get('.form-control').type('l');
         cy.get('.btn-warning').click();
         cy.get('.form-control').clear();
         cy.get('.form-control').type('virág');
         cy.get('.btn-warning').click();
         cy.get('.form-control').clear();
         cy.get('.form-control').type('admin');
         cy.get('.btn-warning').click();
    })

    it('Admin Page',()=>{
        //Admin page átírás
        cy.get(':nth-child(5) > .container-fluid > .row > table > tbody > :nth-child(6) > :nth-child(2) > input').clear();
        cy.get(':nth-child(5) > .container-fluid > .row > table > tbody > :nth-child(6) > :nth-child(2) > input').type('EliLáb');

        cy.get(':nth-child(5) > .container-fluid > .row > table > tbody > :nth-child(6) > :nth-child(3) > input').clear();
        cy.get(':nth-child(5) > .container-fluid > .row > table > tbody > :nth-child(6) > :nth-child(3) > input').type('NOLI');

        cy.get(':nth-child(6) > :nth-child(4) > .checkbox-center').click();
        cy.get(':nth-child(6) > :nth-child(7) > input').click();

        cy.get(':nth-child(5) > .container-fluid > .row > table > tbody > :nth-child(6) > :nth-child(2) > input').clear();
        cy.get(':nth-child(5) > .container-fluid > .row > table > tbody > :nth-child(6) > :nth-child(2) > input').type('Elefántláb');

        cy.get(':nth-child(5) > .container-fluid > .row > table > tbody > :nth-child(6) > :nth-child(3) > input').clear();
        cy.get(':nth-child(5) > .container-fluid > .row > table > tbody > :nth-child(6) > :nth-child(3) > input').type('NOLINA_RECURVATA');

        cy.get(':nth-child(6) > :nth-child(4) > .checkbox-center').click();
        cy.get(':nth-child(6) > :nth-child(7) > input').click();

        cy.get(':nth-child(20) > .container-fluid > .row > table > tbody > :nth-child(2) > :nth-child(1) > input').type('valami@gmail.com');
        cy.get(':nth-child(20) > .container-fluid > .row > table > tbody > :nth-child(2) > :nth-child(2) > input').type('asd');
        cy.get(':nth-child(20) > .container-fluid > .row > table > tbody > :nth-child(2) > :nth-child(3) > .checkbox-center').click();
        cy.get(':nth-child(20) > .container-fluid > .row > table > tbody > :nth-child(2) > .sendlink > input').click();
    })

    it('Új felhasználóval belépés',()=>{
        cy.visit('https://localhost:44377/');

        //Új felhasználóval belépés
        cy.get('form.d-flex > [type="email"]').type('valami@gmail.com');
        cy.get('form.d-flex > [type="password"]').type('asd');
        cy.get('[value="Bejelentkezés"]').click();
    })

    it('Új felhasználó törlése admin page-en',()=>{
        cy.get('.form-control').type('admin');
        cy.get('.btn-warning').click();

        cy.get(':nth-child(23) > .container-fluid > .row > table > tbody > :nth-child(12) > :nth-child(4) > input').click();
    })
})