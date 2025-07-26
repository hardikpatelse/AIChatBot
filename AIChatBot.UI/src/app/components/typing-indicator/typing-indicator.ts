import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core'
import { SignalRService } from '../../services/signalr.service'
import { Subscription } from 'rxjs'

@Component({
  selector: 'app-typing-indicator',
  standalone: false,
  templateUrl: './typing-indicator.html',
  styleUrls: ['./typing-indicator.css']
})
export class TypingIndicatorComponent implements OnInit, OnDestroy {
  status: string = ''
  private statusSub?: Subscription

  constructor(private signalRService: SignalRService, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.statusSub = this.signalRService.status$.subscribe(status => {
      this.status = status
      this.cdr.detectChanges() // Ensure view updates with new status
    })
  }

  ngOnDestroy(): void {
    this.statusSub?.unsubscribe()
  }
}
