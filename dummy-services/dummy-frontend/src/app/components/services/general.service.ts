import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SetProfileDTO } from '../../interfaces/SetProfileDTO';
import { Observable } from 'rxjs';
import { ProfileResponseDTO } from '../../interfaces/ProfileResponseDTO';
import { SetScoreDTO } from '../../interfaces/SetScoreDTO';
import { SetScoreResponseDTO } from '../../interfaces/SetScoreResponseDTO';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GeneralService {

  constructor(private httpClient: HttpClient) { }

  public setProfile(profileData: SetProfileDTO): Observable<ProfileResponseDTO> {
    return this.httpClient.put<ProfileResponseDTO>(`${environment.apiUrl}/set_profile`, profileData);
  }

  public getProfile(profileId: string): Observable<ProfileResponseDTO> {
    return this.httpClient.get<ProfileResponseDTO>(`${environment.apiUrl}/get_profile?id=${profileId}`);
  }

  public setScore(setScoreData: SetScoreDTO): Observable<SetScoreResponseDTO> {
    return this.httpClient.put<SetScoreResponseDTO>(`${environment.apiUrl}/set_score`, setScoreData);
  }
}
