import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeComponent } from './home.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { AuthService } from 'src/app/services/helpers/auth.service';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let router: Router;
  let authService: AuthService;
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[RouterTestingModule,HttpClientTestingModule],
      providers: [HttpClientModule],
      declarations: [HomeComponent],
    });
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    router = TestBed.inject(Router);
    authService = TestBed.inject(AuthService);

  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should navigate to /employee', () => {
    const spyNavigate = spyOn(router, 'navigate');

    component.redirectToEmployees();

    expect(spyNavigate).toHaveBeenCalledWith(['/employee']);
  });
  it('should set userName and trigger change detection', () => {
    const username = undefined;
    spyOn(authService, 'getUserName').and.returnValue(of(username));

    // Trigger ngOnInit manually to start the subscription
    component.ngOnInit();
    fixture.detectChanges();


    // Verify that userName is correctly set and change detection is triggered
    expect(component.userName).toEqual(username);
  });
});
