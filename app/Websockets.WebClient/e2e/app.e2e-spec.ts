import { Websockets.WebClientPage } from './app.po';

describe('websockets.web-client App', () => {
  let page: Websockets.WebClientPage;

  beforeEach(() => {
    page = new Websockets.WebClientPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
