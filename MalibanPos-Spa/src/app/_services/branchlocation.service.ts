import { Injectable } from '@angular/core';
import { environment } from './../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { BranchLocation } from '../models/branchlocation';

@Injectable({
  providedIn: 'root'
})
export class BranchlocationService {
  baseUrl = environment.apiUrl + 'BranchLocation/';
  constructor(private http: HttpClient, private router: Router) { }
  getBranchLocatio(Id: any) {
    return this.http.get<BranchLocation>(this.baseUrl + Id);
  }

}
