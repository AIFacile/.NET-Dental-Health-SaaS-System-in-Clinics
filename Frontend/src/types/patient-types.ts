export interface Patient {
    id: string;
    name: string;
    age: number;
    gender: 'Male' | 'Female' | 'Other';
    lastVisit: string;
    status: 'Healthy' | 'Under Treatment' | 'Follow-up' | 'Emergency';
    phone: string;
  }