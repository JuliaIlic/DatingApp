import { Component, inject, input, OnInit } from '@angular/core';
import { Member } from '../../_models/member';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { MemberDetailComponent } from '../member-detail/member-detail.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-member-card',
  standalone: true,
  imports: [ CommonModule, RouterLink],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css'
})
export class MemberCardComponent implements OnInit{
  ngOnInit(): void {
    console.log('Member in card:', this.member());
  }
  member = input.required<Member>();
}
