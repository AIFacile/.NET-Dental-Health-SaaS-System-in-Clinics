export interface LoginRequest {
    tenantCode: string;
    username: string;
    password: string;
  }
  
  export interface LoginResponse {
    accessToken: string;
  }

  export enum RoleName {
    SuperAdmin = 'SuperAdmin',
    TenantAdmin = 'TenantAdmin',
    Doctor = 'Doctor',
    Assistant = 'Assistant',
    Receptionist = 'Receptionist',
    Accountant = 'Accountant'
  }
  
  export interface DecodedToken {
    sub: string;
    tenant_id: string;
    exp: number;
    permissions: string[];
    // Mapping Microsoft's long claim names
    'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name': string;
    'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': RoleName;
  }
  
  export interface UserSession {
    username: string;
    role: RoleName;
    permissions: string[];
    tenantId: string;
  }