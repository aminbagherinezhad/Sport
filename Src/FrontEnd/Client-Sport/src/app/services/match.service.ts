import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Match } from '../Models/match.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MatchService {
  private baseUrl = 'https://localhost:7130/api/Home/Home'; // آدرس API بک‌اند

  constructor(private http: HttpClient) {}

  getAllMatches(): Observable<Match[]> {
    return this.http.get<Match[]>(`${this.baseUrl}/Home`);
  }
}
