import { ChangeDetectorRef, Component } from '@angular/core';
import { AuthService } from './services/helpers/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'AngularCivicaEmployeeMaster';
  isAuthenticated: boolean = false;
  userName:string|null|undefined;

  constructor(private authService:AuthService,private cdr:ChangeDetectorRef) {}
  ngOnInit(): void {
    this.authService.isAuthenticated().subscribe((authState:boolean)=>{
      this.isAuthenticated = authState;
      this.cdr.detectChanges();//Manually trigger change detection
    });
    this.authService.getUserName().subscribe((username:string|null|undefined)=>{
      this.userName = username;
      this.cdr.detectChanges();
    })
  }

  signOut(){
    this.authService.signOut();
  }
}
