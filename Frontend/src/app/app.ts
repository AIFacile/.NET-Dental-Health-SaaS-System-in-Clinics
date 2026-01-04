import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { LoginComponent } from "../features/auth/login/login.component";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private http = inject(HttpClient);
  protected router = inject(Router);
  protected title = '<"Dental.Health">';

  async ngOnInit() {
    this.http.get('https://localhost:7168/api/auth/login')
  }
}
