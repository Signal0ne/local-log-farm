import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GeneralService } from '../services/general.service';
import { FormsModule } from '@angular/forms';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-general-component',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './general-component.html',
  styleUrl: './general-component.scss'
})
export class GeneralComponent {
  public setProfileName!: string;
  public setProfileEmail!: string;
  public getProfileId!: string;
  public setScoreUserProfile!: string;
  public setScoreScore!: number;
  private completeInterval!: any;
  private setProfileInterval!: any;
  private getProfileInterval!: any;
  private setScoreInterval!: any;
  constructor(private generalService: GeneralService) {
  }

  public getProfile(): void {
    this.generalService.getProfile(this.getProfileId).subscribe((res) => console.log(res), err => console.error(err))
  }

  public setProfile(): void {
    this.generalService.setProfile({Name: this.setProfileName, Email: this.setProfileEmail}).subscribe((res) => console.log(res), err => console.error(err))

  }

  public setScore(): void {
    this.generalService.setScore({Score: this.setScoreScore, UserProfile: this.setScoreUserProfile}).subscribe((res) => console.log(res), err => console.error(err))
  }

  public startGetProfileInterval(): void {
    this.clearGetProfileInterval()
    this.getProfileInterval = setInterval(() => {
      this.getProfile();
    }, environment.getProfileIntervalTime)
  }

  public clearGetProfileInterval(): void {
    if (this.getProfileInterval) {
      clearInterval(this.getProfileInterval);
      this.getProfileInterval = null;
    }
  }

  public startSetProfileInterval(): void {
    this.clearSetProfileInterval();
    this.setProfileInterval = setInterval(() => {
      this.setProfile();
    }, environment.setProfileIntervalTime)

  }

  public clearSetProfileInterval(): void {
    if (this.setProfileInterval) {
      clearInterval(this.setProfileInterval);
      this.setProfileInterval = null;
    }
  }

  public startSetScoreInterval(): void {
    this.clearSetScoreInterval();
    this.setScoreInterval = setInterval(() => {
      this.getProfile();
      this.setProfile();
      this.setScore();
    }, environment.setScoreIntervalTime)

  }

  public clearSetScoreInterval(): void {
    if (this.setScoreInterval) {
      clearInterval(this.setScoreInterval);
      this.setScoreInterval = null;
    }
  }

  public startCompleteInterval(): void {
    this.clearCompleteInterval();
    this.completeInterval = setInterval(() => {
      this.getProfile();
      this.setProfile();
      this.setScore();
    }, environment.completeIntervalTime)

  }

  public clearCompleteInterval(): void {
    this.clearSetProfileInterval();
    this.clearSetScoreInterval();
    this.clearSetProfileInterval();
    if (this.completeInterval) {
      clearInterval(this.completeInterval);
      this.completeInterval = null;
    }
  }

}
