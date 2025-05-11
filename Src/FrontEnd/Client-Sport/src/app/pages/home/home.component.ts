import { Component, OnInit } from '@angular/core';
import { MatchService } from '../../services/match.service';
import { Match } from '../../Models/match.model';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  template: `
    <h2>لیست مسابقات</h2>
    <ul *ngIf="matches">
      <li *ngFor="let match of matches">
        <h3>{{ match.title }}</h3>
        <p>{{ match.description }}</p>
      </li>
    </ul>
  `
})
export class HomeComponent implements OnInit {
  matches: Match[] = [];

  constructor(private matchService: MatchService) {}

  ngOnInit() {
    this.matchService.getAllMatches().subscribe({
      next: (data) => {
        this.matches = data;
      },
      error: (err) => {
        console.error('خطا در دریافت مسابقات:', err);
      }
    });
  }
}
