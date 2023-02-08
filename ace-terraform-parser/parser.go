package main

import "C"

import (
	"context"
	"encoding/json"
	"log"
	"os"
	"os/exec"

	"github.com/hashicorp/terraform-exec/tfexec"
)

//export GenerateParsedPlan
func GenerateParsedPlan(workingDir string, planFile string) {
	path, err := exec.LookPath("terraform")
	tf, err := tfexec.NewTerraform(workingDir, path)
	if err != nil {
		log.Fatalf("error running NewTerraform: %s", err)
	}

	plan, err := tf.ShowPlanFile(context.Background(), planFile)
	if err != nil {
		log.Fatalf("error running Show: %s", err)
	}

	ret, err := json.Marshal(plan)
	os.WriteFile("ace-terraform-parsed-plan.json", ret, 0644)
}

func main() {
}
