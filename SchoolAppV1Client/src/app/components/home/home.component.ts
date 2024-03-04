import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm, NgModel } from '@angular/forms';

import { ClassRoomModel } from '../../Models/class-room.model';
import { StudentModel } from '../../Models/student.model';
import { StudentPipe } from '../pipes/student.pipe';
import { HttpService } from '../../services/http.service';
import { FormValidateDirective } from 'form-validate-angular';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule, StudentPipe,FormValidateDirective],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  classRooms: ClassRoomModel[] = [];
  students: StudentModel[] = [];

  addStudentModel:StudentModel = new StudentModel();
  updatetudentModel:StudentModel = new StudentModel();

  selectedRoomId: string = "";
  search: string = "";


  constructor(
    private http: HttpService) {
    this.getAllClassRooms();
  }

  getAllClassRooms() {
    this.http.get("ClassRooms/GetAll", (res) => {
      this.classRooms = res;

      if (this.classRooms.length > 0) {
        this.getAllStudentsByClassRoomId(this.classRooms[0].id);
        //this.selectedRoomId = this.classRooms[0].id;
      }
    });
  }

  getAllStudentsByClassRoomId(roomId: string) {
    this.selectedRoomId = roomId;
    this.http.get("Students/GetAllByClassRoomId?classRoomId=" + this.selectedRoomId, res => {
      this.students = res

      this.students = this.students.map((val) => {
        const identityNumberPart1 = val.identityNumber.substring(0, 2);
        const identityNumberPart2 = val.identityNumber.substring(val.identityNumber.length - 6, 3);

        const newHashedIdentityNumber = identityNumberPart1 + "******" + identityNumberPart2;

        val.identityNumber = newHashedIdentityNumber;

        return val;
      })

    });
  }

  createStudent(from:NgForm){
    if(from.valid){
      if(this.addStudentModel.classRoomId == "0"){
        alert("You must select a class room");
        return;
      }
      this.http.post("Students/Create",this.addStudentModel, (res)=>{

      })

    }
  }
}
