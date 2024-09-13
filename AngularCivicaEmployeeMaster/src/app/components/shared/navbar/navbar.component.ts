import { ChangeDetectorRef, Component } from '@angular/core';
import { Route, Router } from '@angular/router';
import { AuthService } from 'src/app/services/helpers/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
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
  signOut(){
    this.authService.signOut();
  }

  reportDecider(reportDeciderVal:string){
    this.router.navigate(['/reportdecider/', reportDeciderVal]);
  }
}
