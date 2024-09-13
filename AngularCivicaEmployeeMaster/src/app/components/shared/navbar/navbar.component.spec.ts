import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavbarComponent } from './navbar.component';
import { HttpClientModule } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/helpers/auth.service';

describe('NavbarComponent', () => {
  let component: NavbarComponent;
  let fixture: ComponentFixture<NavbarComponent>;
  let router: Router;
  let authService: AuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports : [HttpClientTestingModule,RouterTestingModule,FormsModule],
      
      providers: [HttpClientModule],
      declarations: [NavbarComponent]
    });
    fixture = TestBed.createComponent(NavbarComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should navigate to employee details', () => {
    router = TestBed.inject(Router); // Inject Router for navigation testing
    const navigateSpy = spyOn(router, 'navigate').and.returnValue(Promise.resolve(true));
  
    const reportDeciderVal = 'all';
    component.reportDecider(reportDeciderVal);
  
    expect(navigateSpy).toHaveBeenCalledWith(['/reportdecider/', reportDeciderVal]);
  });
  it('should call authService.signOut()', () => {
    const spySignOut = spyOn(authService, 'signOut').and.callThrough();

    component.signOut();

    expect(spySignOut).toHaveBeenCalled();
  });

 
});
