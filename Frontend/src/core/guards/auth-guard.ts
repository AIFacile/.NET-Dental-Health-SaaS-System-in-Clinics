import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth-service';
import { RoleName } from '../../types/auth-types';
import { concatWith } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Check if the user is logged in
  if (!authService.isLoggedIn()) {
    return router.createUrlTree(['/login']);
  }

  // Check for Role Requirements
  const expectedRoles = route.data['roles'] as Array<RoleName>;
  const user = authService.currentUser();

  if (expectedRoles && user) {
    const hasRole = expectedRoles.includes(user.role);

    if (!hasRole) {
      // User is logged in but doesn't have the right role
      console.warn('Access denied: Unauthorized role');
      return router.createUrlTree(['/login']);
    }
  }

  return true;
};
