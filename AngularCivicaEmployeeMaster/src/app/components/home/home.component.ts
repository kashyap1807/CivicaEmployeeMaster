import { ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/helpers/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  isAuthenticated: boolean = false;
  userName:string|null|undefined;

  constructor(private authService:AuthService,private cdr:ChangeDetectorRef,private router:Router) {}
  ngOnInit(): void {
    this.authService.isAuthenticated().subscribe((authState:boolean)=>{
      this.isAuthenticated = authState;
      if(this.isAuthenticated)
        {
        this.authService.getUserName().subscribe((username:string|null|undefined)=>{
          this.userName = username;
          this.cdr.detectChanges();
        });

      }
      this.cdr.detectChanges();//Manually trigger change detection
    });
  }

  redirectToEmployees() {
    this.router.navigate(['/employee']);
  }

}
