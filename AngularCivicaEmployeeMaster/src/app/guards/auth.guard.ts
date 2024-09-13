import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { map } from 'rxjs';
import { AuthService } from '../services/helpers/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  return authService.isAuthenticated().pipe(
    map(isAuthenticated=>{
      if(isAuthenticated){
        return true;
      }else{
        router.navigate(['/signin']);
        return false;
      }
    })
  );
};
