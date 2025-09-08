// auth-role.guard.ts
import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router
} from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {
  constructor(private auth: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    // route.data.roles can be a string or array of strings
    const expected = route.data['roles'];
    const allowedRoles: string[] = Array.isArray(expected) ? expected : (expected ? [expected] : []);

    // 1) Check logged in
    if (!this.auth.isLoggedIn()) {
      // optionally preserve returnUrl
      this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
      return false;
    }

    // 2) If no role constraint set on route, allow
    if (!allowedRoles || allowedRoles.length === 0) return true;

    // 3) Check user's role
    const userRole = this.auth.getUserRole();
    if (userRole && allowedRoles.includes(userRole)) return true;

    // 4) Not authorized -> redirect (adjust as you prefer)
    this.router.navigate(['/']); // landing or "not-authorized" page
    return false;
  }
}
