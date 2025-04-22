import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientBooksSearchComponent } from './client-books-search.component';

describe('ClientBooksSearchComponent', () => {
  let component: ClientBooksSearchComponent;
  let fixture: ComponentFixture<ClientBooksSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClientBooksSearchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClientBooksSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
