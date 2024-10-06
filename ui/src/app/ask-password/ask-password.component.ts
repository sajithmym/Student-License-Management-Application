import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-ask-password',
  templateUrl: './ask-password.component.html',
  styleUrls: ['./ask-password.component.css']
})
export class AskPasswordComponent {
  @Input() show: boolean = false;
  @Output() close = new EventEmitter<void>();
  @Output() submit = new EventEmitter<string>();

  password: string = '';

  onClose() {
    this.close.emit();
  }

  onSubmit() {
    this.submit.emit(this.password);
  }
}
